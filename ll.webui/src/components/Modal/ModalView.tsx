import React, { useState, forwardRef, useImperativeHandle } from 'react';
import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';

import './ModalView.css'; 

interface ModalViewProps {
  modalTitle: string;
  modalBody: string;
  onClick?: Function;
}

const ModalView = forwardRef(({ modalTitle, modalBody, onClick}: ModalViewProps, ref) => {

  const [showModal, setShowModal] = useState(false);

  const openModal = () => setShowModal(true);
  const closeModal = () => setShowModal(false);

  const Confirm = () => {

    if(onClick != null)
      onClick();
    
    closeModal();
  };

  useImperativeHandle(ref, () => ({
    openModal,
    closeModal
  }));

  return (
    <>
      <Modal show={showModal} onHide={closeModal}>
        <Modal.Header closeButton>
          <Modal.Title>{modalTitle}</Modal.Title>
        </Modal.Header>
        <Modal.Body>{modalBody}</Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={closeModal}>
            Close
          </Button>
          <Button variant="primary" onClick={Confirm}>
             OK
          </Button>
        </Modal.Footer>
      </Modal>
    </>
  );
});

export default ModalView;
